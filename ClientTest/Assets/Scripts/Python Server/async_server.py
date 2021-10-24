import asyncio
import argparse 
import socket
import threading

from time import sleep

HOST, PORT = 'localhost', 7777

class ServerTCP(asyncio.Protocol):

    def __init__(self):
        self.clients = []
        super().__init__()

    def connection_made(self, transport):
        self.clients.append(transport)
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
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
    
    def datagram_received(self, data, addr):
        print(f"Received Syslog message: {data} from {addr}")
        self.transport.sendto(b"Recu 5/5 client", addr)
        response = b"Instantiate Player"
        self.transport.sendto(response, addr)
    


if __name__ == '__main__':
    loop = asyncio.get_event_loop()
    #t = loop.create_server(ServerTCP, host='0.0.0.0', port=PORT)
    t = loop.create_datagram_endpoint(ServerUDP, local_addr=('0.0.0.0', PORT))
    loop.run_until_complete(t) # Server starts listening
    loop.run_forever()