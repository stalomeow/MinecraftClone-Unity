'''
author: Jin Yuhan
date: 2020-12-25 23:56:42
lastTime: 2020-12-26 15:37:39
'''


class ArduinoPackage(object):
    def __init__(self, acceleration, angularVelocity, rotation, leftPressures, rightPressures):
        self.acceleration = acceleration
        self.angularVelocity = angularVelocity
        self.rotation = rotation
        self.leftPressures = leftPressures
        self.rightPressures = rightPressures

    def __str__(self):
        return "Package:\n\tacceleration: {}\n\tangularVelocity: {}\n\trotation: {}\n\tleft: {}\n\tright: {}"\
            .format(self.acceleration, self.angularVelocity, self.rotation, self.leftPressures, self.rightPressures)
