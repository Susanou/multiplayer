import asyncio
import argparse 
import socket
import threading

from player import Player
from time import sleep

HOST, PORT = 'localhost', 7777

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


class ServerUDP(asyncio.DatagramProtocol):
    
    def __init__(self):
        self.connections = []
        self.players = {}
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
    
    def datagram_received(self, data, addr):
        
        if addr not in self.connections:
            self.connections.append(addr)
        print(f"Received Syslog message: {data} from {addr}")
        
        if data == b'Ping':
            self.welcome_msg(addr)
            self.create_player(addr)
        
        if data == b'Disconnected':
            self.connections.remove(addr)
            del self.players[addr]
    
    def welcome_msg(self, addr):
        response = f'W:{self.connections.index(addr)}'.encode()
        self.transport.sendto(response, addr)

    def send_to_all(self, data):
        for a in self.connections:
            self.transport.sendto(data, a)


    def create_player(self, addr):
        
        player = Player(self.connections.index(addr), True)
        self.players[addr] = player
        print(f"player {player.id} has been added to the game")
        self.join_game(addr, player)

    def player_data(self, addr, player):
        response = b'I:'
        response += str(self.connections.index(addr)).encode()

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
    loop = asyncio.get_event_loop()
    #t = loop.create_server(ServerTCP, host='0.0.0.0', port=PORT)
    t = loop.create_datagram_endpoint(ServerUDP, local_addr=('0.0.0.0', PORT))
    loop.run_until_complete(t) # Server starts listening
    loop.run_forever()