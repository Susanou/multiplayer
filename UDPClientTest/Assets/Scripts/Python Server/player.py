class Point():

    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def __str__(self):
        return '='+str(self.x) + ';' + str(self.y) + ';' + str(self.z)

Spawn = Point(0,0,0)

class Player():
    
    def __init__(self, id, in_game, rotation=None, position=None):
        self.id = id
        self.in_game = in_game
        self.position = Point(0,0,0) if position==None else position
        self.rotation = 0 if rotation==None else rotation

    def __str__(self):
        return str(self.id) + '=' + str(self.position.x) + ';' + str(self.position.y) + ';' + str(self.position.z)


