'''
author: Jin Yuhan
date: 2020-12-19 15:49:32
lastTime: 2020-12-26 15:43:37
'''

import xlwt
from arduino_reader import ArduinoReader

def createExcel():
    workbook = xlwt.Workbook(encoding = 'utf-8')
    worksheet = workbook.add_sheet('Sample Data')
    items = [
        "加速度 X", "加速度 Y", "加速度 Z",
        "角速度 X", "角速度 Y", "角速度 Z",
        "滚转角（X）", "俯仰角（Y）", "偏航角（Z）",
        "压力 A0", "压力 A1", "压力 A2", "压力 A3", "压力 A4", "压力 A5",
    ]

    for i in range(len(items)):
        worksheet.write(0, i, items[i])

    return workbook, worksheet

def writeListToExcel(worksheet, line, column, items):
    for i in range(len(items)):
        worksheet.write(line, column + i, items[i])

def writeDataToExcel(worksheet, line, data):
    writeListToExcel(worksheet, line, 0, data.acceleration)
    writeListToExcel(worksheet, line, 3, data.angularVelocity)
    writeListToExcel(worksheet, line, 6, data.rotation)
    writeListToExcel(worksheet, line, 9, data.leftPressures)
    writeListToExcel(worksheet, line, 12, data.rightPressures)

arduinoConfig = {
    "port": "COM6",
    "baudrate": 9600,
    "timeout": 1,
    "gravity": 9.8,
    "packageFlag0": 0x55,
    "packageFlag1": 0x59,
    "packageBodySize": 30
}

if __name__ == '__main__':
    with ArduinoReader(**arduinoConfig) as reader:
        workbook, worksheet = createExcel()

        line = 1
        while(True):
            package = reader.read()
            writeDataToExcel(worksheet, line, package)
            print(package)
            line += 1

            if line % 50 == 0:
                workbook.save('Samples.xls')
