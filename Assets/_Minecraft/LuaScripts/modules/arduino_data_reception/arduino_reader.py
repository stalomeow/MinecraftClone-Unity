'''
author: Jin Yuhan
date: 2020-12-25 23:35:10
lastTime: 2020-12-26 15:44:56
'''

from serial import Serial
from arduino_package import ArduinoPackage


class CStream(object):
    def __init__(self, cbytes: bytes):
        self.__cbytes = cbytes
        self.__index = 0

    def __len__(self):
        return len(self.__cbytes)

    def __readTwoBytes(self):
        # 一个数，用两个字节传输
        # Arduino 端是 Little Endian
        low = self.__cbytes[self.__index]
        high = self.__cbytes[self.__index + 1]
        self.__index += 2
        return low, high

    def readInt(self, signed=True):
        low, high = self.__readTwoBytes()

        if signed:
            v = ((high & 0x7F) << 8) | low
            return v if (high & 0x80) == 0 else -((~(v - 1)) & 0x7FFF)
        else:
            return (high << 8) | low
        
    def readFloat(self):
        return self.readInt(signed=True) / 32768.0

    def readIntVec(self, elementDecorator, dimension=3, signed=True):
        return [
            elementDecorator(i, self.readInt(signed=signed))
            for i in range(dimension)
        ]

    def readFloatVec(self, elementDecorator, dimension=3):
        return [
            elementDecorator(i, self.readFloat())
            for i in range(dimension)
        ]


class ArduinoReader(object):
    def __init__(self, **kwargs):
        port = kwargs.get('port', None)
        baudrate = kwargs.get('baudrate', 9600)
        timeout = kwargs.get('timeout', 1)
        
        self.__serial = Serial(port=port, baudrate=baudrate, timeout=timeout)
        self.gravity = kwargs.get('gravity', 9.8)
        self.packageFlag0 = kwargs.get('packageFlag0', 0x55)
        self.packageFlag1 = kwargs.get('packageFlag1', 0x59)
        self.packageBodySize = kwargs.get('packageBodySize', 30)

    def __readSingleByte(self):
        data = self.__serial.read(1)
        return data[0] if len(data) == 1 else None

    def __checkPackageFlags(self):
        previous = self.__readSingleByte()

        while True:
            current = self.__readSingleByte()
            # 前面两个字节是标识位
            if previous == self.packageFlag0 and current == self.packageFlag1:
                return
            else:
                previous = current

    def __makePackage(self, cbytes):
        if len(cbytes) != self.packageBodySize:
            return None

        stream = CStream(cbytes)

        accel = stream.readFloatVec(lambda i, v: round(v * 16 * self.gravity, 4)) # m/(s**2)
        angularV = stream.readFloatVec(lambda i, v: round(v * 2000, 4))           # DEG/s
        rotation = stream.readFloatVec(lambda i, v: round(v * 180, 4))            # DEG
        
        left = stream.readIntVec(lambda i, v: (v), signed=False)
        right = stream.readIntVec(lambda i, v: (v), signed=False)

        return ArduinoPackage(accel, angularV, rotation, left, right)

    def open(self):
        self.__serial.open()

    def close(self):
        self.__serial.close()

    def __enter__(self):
        self.__serial.__enter__()
        return self

    def __exit__(self, extype, value, trace):
        self.__serial.__exit__(extype, value, trace)

    @property
    def canRead(self):
        return self.__serial.is_open

    def read(self):
        if not self.canRead:
            return None

        self.__checkPackageFlags()
        cbytes = self.__serial.read(self.packageBodySize)
        return self.__makePackage(cbytes)
