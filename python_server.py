import socket 
import argparse 
import socket
import threading

from time import sleep

# Define socket host and port 
server_socket = None
HOST_ADDR = "0.0.0.0"
HOST_PORT = 7777
client_name = " "
clients = []
clients_names = []
player_data = []


def server():
    global server_socket
    # Create socket 
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1) 
    server_socket.bind((HOST_ADDR, HOST_PORT)) 
    server_socket.listen() 
    print('Listening on port %s ...' % HOST_PORT) 
    threading._start_new_thread(accept_clients, (server_socket))

def accept_clients(server, y):
    while True:
        client, addr = server.accept()
        clients.append(client)

        threading._start_new_thread(messaging, (client, addr))

def messaging(client_conn, client_addr):
    
    reply = b""

    while True:
        try:
            data = client_conn.recv(2048)
            
            if not data:
                print("Client disconnected from server")
                break
            elif b"close" in data:
                close(server_socket)
                break
            else:
                print("received message from client: ", data)
            
            client_conn.sendall(b'received')
        except Exception as e:
            print("Something went wrong: ", e)
            break
    
    client_conn.close()

def close(server):
    server.shutdown(socket.SHUT_RDWR)
    server.close()
    print ("closed")
 
# Close socket 
#server_socket.close()

while True:
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1) 
    server_socket.bind((HOST_ADDR, HOST_PORT)) 
    server_socket.listen() 
    print('Listening on port %s ...' % HOST_PORT) 

    while True:
        client, addr = server_socket.accept()
        #clients.append(client)

        threading._start_new_thread(messaging, (client, addr))