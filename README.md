
# websocket-windows-reverse-shell

This is repository for Websocket Reverse Shell that can be executed under Microsoft Windows OS. The solution is split to two components **client.cs**, which is C# reverse shell code that is executed on client-side and and **server.py**, which serves as python server side components that control commands and allow to send requests to client.

## Usage

#### Step 1: Generate a Private Key

Run the following command to create a **2048-bit RSA private key**:
```
openssl genpkey -algorithm RSA -out server.key -pkeyopt rsa_keygen_bits:2048
```

#### Step 2: Generate a Certificate Signing Request (CSR)

Then run this command to generate CSR:
```
openssl req -new -key server.key -out server.csr
```
#### Step 3: Generate a Self-Signed Certificate

Then this:
```
openssl x509 -req -days 365 -in server.csr -signkey server.key -out server.crt
```

#### Step 4: Configure Server Address, Port and Key Location

Change Host, Port in server.py and change location for your generated server.crt and server.key.

#### Step 5: Modify IP Address and Port in Client

Change client.js IP address and port that matches your server config.

You can now compile client.cs and launch server.py.
