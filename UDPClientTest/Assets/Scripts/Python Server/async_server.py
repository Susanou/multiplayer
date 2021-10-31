import asyncio
import logging
import logging.config

from struct import *

from player import Player
from time import sleep
from inputs import *

HOST, PORT = 'localhost', 7777
logging.config.fileConfig('logging.conf')
logger = logging.getLogger('simpleExample')

class ServerTCP(asyncio.Protocol):
    
    def __init__(self):
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
        self.transport.write(b"1Welcome to the server!\n")
    
    def data_received(self, data):
        print(f"Received Syslog message: {data}")
        response = b"Recu 5 sur 5\n"
        self.transport.write(response)
    
    def connection_lost(self, exec):
        print("Connection closed")
        self.transport = None

class ServerUDP2(asyncio.DatagramProtocol):
    
    def __init__(self):
        self.connections = []
        self.players = {}
        self.speed = 0.1
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
    
    def datagram_received(self, data, addr):

        super().datagram_received(data, addr)
        
        #self.client_connection(data, addr)
        loop = asyncio.get_event_loop()
        loop.create_task(self.client_connection(data, addr))
    
    async def client_connection(self, data, addr):

        if addr not in self.connections:
            self.connections.append(addr)
        #print(f"Received Syslog message: {data} from {addr}")
        connectionID = str(self.connections.index(addr))

        if data == b'Ping':
            asyncio.create_task(self.welcome_msg(addr))
            asyncio.create_task(self.create_player(addr))
        elif data == b'Disconnected':
            self.connections.remove(addr)
            del self.players[str(connectionID)]
        else:
            datagram = unpack('<cxc', data)

            if b'M' == datagram[0]:
                move = datagram[1]

                logger.debug(f"Moving Player {connectionID} {data.decode()}")
                
                response = b'P:'
                movement = try_to_move(self.players[connectionID].position, move, self.speed)
                response += connectionID.encode() + movement.__str__().encode()
                [logger.debug(f"{k}, position , {v}") for k,v in self.players.items()]
                self.players[connectionID].position = movement
                [logger.debug(f"{k}, position after movement , {v}") for k,v in self.players.items()]
                asyncio.create_task(self.send_to_all(response))

    
    async def welcome_msg(self, addr):
        response = f'W:{self.connections.index(addr)}'.encode()
        self.transport.sendto(response, addr)

    async def send_to_all(self, data):
        #logger.debug(data)
        for a in self.connections:
            self.transport.sendto(data, a)

    async def create_player(self, addr):
        connectionID = str(self.connections.index(addr))
        player = Player(connectionID, True)
        logger.debug(f"player {player.id} has been added to the game")
        self.players[connectionID] = player
        asyncio.create_task(self.join_game(addr, player))
        

    def player_data(self, connectionID, player):
        response = b'I:'
        response += connectionID.encode()

        return response


    async def join_game(self, addr, player):
        self.instantiate_player(addr, player)
    
    def instantiate_player(self, addr, player:Player):
        connectionID = str(self.connections.index(addr))
        # Send all connected players to the new connection
        for i in self.players:
            if self.players[i] is not None:
                if self.players[i].in_game:
                    if i != connectionID:
                        self.transport.sendto(self.player_data(i, self.players[i]), addr)

        #send the new player data to all clients
        asyncio.create_task(self.send_to_all(self.player_data(connectionID, player)))

class ServerUDP(asyncio.DatagramProtocol):
    
    def __init__(self):
        self.connections = []
        self.players = {}
        self.speed = 0.1
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
    
    def datagram_received(self, data, addr):

        super().datagram_received(data, addr)
        
        self.client_connection(data, addr)
        #this_loop = asyncio.get_event_loop()
        #this_loop.create_task(self.client_connection(data, addr))
    
    def client_connection(self, data, addr):

        if addr not in self.connections:
            self.connections.append(addr)
        print(f"Received Syslog message: {data} from {addr}")
        
        if data == b'Ping':
            self.welcome_msg(addr)
            self.create_player(addr)
        elif data == b'Disconnected':
            connectionID = self.connections.index(addr)
            self.connections.remove(addr)
            del self.players[str(connectionID)]
        else:
            datagram = unpack('<cxc', data)

            if b'M' == datagram[0]:
                move = datagram[1]
                connectionId = str(self.connections.index(addr))
                response = b'P:'
                movement = try_to_move(self.players[connectionId].position, move, self.speed)
                response += connectionId.encode() + movement.__str__().encode()
                self.send_to_all(response)

    
    def welcome_msg(self, addr):
        response = f'W:{self.connections.index(addr)}'.encode()
        self.transport.sendto(response, addr)

    def send_to_all(self, data):
        for a in self.connections:
            self.transport.sendto(data, a)

    def create_player(self, addr):
        connectionID = str(self.connections.index(addr))
        player = Player(connectionID, True)
        self.players[connectionID] = player
        print(f"player {player.id} has been added to the game")
        self.join_game(connectionID, player)

    def player_data(self, connectionID, player):
        response = b'I:'
        response += connectionID.encode()

        return response


    def join_game(self, addr, player):
        self.instantiate_player(addr, player)
    
    def instantiate_player(self, addr, player:Player):
        
        # Send all connected players to the new connection
        for i in self.players:
            if self.players[i] is not None:
                if self.players[i].in_game:
                    if i != addr:
                        self.transport.sendto(self.player_data(i, self.players[i]), addr)

        self.send_to_all(self.player_data(addr, player))


if __name__ == '__main__':
    game_loop = asyncio.get_event_loop()
    #t = loop.create_server(ServerTCP, host='0.0.0.0', port=PORT)
    t = game_loop.create_datagram_endpoint(ServerUDP2, local_addr=('0.0.0.0', PORT))
    game_loop.run_until_complete(t) # Server starts listening
    game_loop.run_forever()