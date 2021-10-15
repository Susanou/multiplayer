import asyncio
import argparse 
import socket
import threading

from time import sleep

HOST, PORT = 'localhost', 7777

class Server(asyncio.DatagramProtocol):

    def __init__(self):
        super().__init__()

    def connection_made(self, transport):
        self.transport = transport
        print("Connection made with: ", transport)
    
    def datagram_received(self, data, addr):
        print(f"Received Syslog message: {data}")


if __name__ == '__main__':
    loop = asyncio.get_event_loop()
    t = loop.create_datagram_endpoint(Server, local_addr=('0.0.0.0', PORT))
    loop.run_until_complete(t) # Server starts listening
    #loop.run_until_complete(write_messages()) # Start writing messages (or running tests)
    loop.run_forever()