from player import *

def try_to_move(position,  key, speed):

    tmp_position = position

    if key == b'1':
        tmp_position.x += speed
        tmp_position.z += speed
    elif key == b'2':
        tmp_position.x -= speed
        tmp_position.z -= speed
    elif key == b'3':
        tmp_position.x -= speed
        tmp_position.z += speed
    elif key == b'4':
        tmp_position.x += speed
        tmp_position.z -= speed

    return tmp_position

