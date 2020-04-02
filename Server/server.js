const WebSocket = require('ws')
class Server{
    constructor(port){
        this.clients = []
        this.users = []
        this.usersUniqueID = 1
        this.ws = new WebSocket.Server({port:port})
        this.ws.on('connection',this.connectionListener.bind(this))
        p(`server running at port ${port}\n`)
        this.time = 0;
    }
    connectionListener(ws,require){
        this.clients = this.clients.filter(c => c.readyState === 1)
        ws.name = ws._socket.remoteAddress + ":" + `${Math.random()}`.slice(2,4)
        this.clients.push(ws)
        p(`Join ${ws.name}`)
        this.emit(ws,{Type: 'join'})

        ws.on('message',data => {
            let d = JSON.parse(data)
            
            switch(d.Type){
                case 'updateUser':
                    let msg = JSON.parse(d.Data)
                    this.users = this.users.filter(c => c.ID !== msg.User.ID)
                    this.users.push(msg.User)
                    this.broadcast(ws,data)
                    break
                case 'gameStart':
                    let c = this.createUser(d.Data,ws.name)
                    this.users.push(c)
                    this.broadcast(ws,JSON.stringify({
                        Type: 'gameStart',
                        Data: JSON.stringify({
                            Users:this.users,
                            Player:c,
                        }),
                    }))
                break
                default:
                    this.broadcast(ws,data)
            }
        })
        ws.on('close',() => {
            this.users = this.users.filter(c => c.WsName !== ws.name)
            this.clients.slice(this.clients.indexOf(ws),1)
            this.broadcast(ws,JSON.stringify({
                Type: 'exitUser',
                Data: JSON.stringify({
                    WsName: ws.name,
                })
            }))
            p(`Exit ${ws.name}`)
        })
    }
    createUser(name,WsName){
        return{
            ID: this.usersUniqueID++,
            WsName: WsName === undefined? "" : WsName,
            Name: name === undefined? "" : name,
            Hp: 400,
            Dmg: 100,
            X: -1.8 + Math.random() * 3.6,
            Y: 1.5,
            Z: -1.8 + Math.random() * 3.6,
            IsSneak: false,
            SneakName: "Human",
            tokuten: 0
        }
    }
    emit(ws, data){
      ws.send(JSON.stringify(data))
    }
    /** Push to other clients */
    broadcast(sender, message) {
        let aaa = 0;
      for (let c of this.clients) {
        if (c.readyState === 1) {
            aaa++;
          c.send(message)
          //p(`aaa => ${aaa}`)
          //p(`message => ${message}`)
        }
      }
    }
    /** close server */
    close() {
      this.server.close()
    }
}
function p(message) {
  process.stdout.write(message + '\n')
}
module.exports = Server