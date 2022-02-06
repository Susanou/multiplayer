from player import *
import math

def try_to_move(position,  key, speed, rotation):

    tmp_position = position

    if key == b'1':
        tmp_position.x += speed * convert_rotation_sin(rotation)
        tmp_position.z += speed * convert_rotation_cos(rotation)
    elif key == b'2':
        tmp_position.x -= speed * convert_rotation_sin(rotation)
        tmp_position.z -= speed * convert_rotation_cos(rotation)
    elif key == b'3':
        tmp_position.x -= speed * convert_rotation_sin(rotation)
        tmp_position.z += speed * convert_rotation_cos(rotation)
    elif key == b'4':
        tmp_position.x += speed * convert_rotation_sin(rotation)
        tmp_position.z -= speed * convert_rotation_cos(rotation)

    return tmp_position

def convert_rotation_sin(rotation):
    return round(math.sin(rotation * (math.pi / 180)), 4);

def convert_rotation_cos(rotation):
    return round(math.cos(rotation * (math.pi / 180)), 4);