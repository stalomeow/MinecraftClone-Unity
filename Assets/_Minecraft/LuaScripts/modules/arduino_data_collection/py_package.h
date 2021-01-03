/*
 * @author: Jin Yuhan
 * @date: 2020-12-19 18:28:13
 * @lastTime: 2020-12-26 14:17:30
 */

#ifndef PY_PACKAGE_H
#define PY_PACKAGE_H

#include "gyro.h"
#include "pressures.h"

/**
 * 数据包的第一个标识
 */
#define PACKAGE_FLAG_0 0x55

/**
 * 数据包的第二个标识
 */
#define PACKAGE_FLAG_1 0x59

/**
 * 发送给python的数据包格式
 */
struct PyPackage
{
  unsigned char Flag0;                                /* 第一个标识 */
  unsigned char Flag1;                                /* 第二个标识 */
  GyroData GyroData;                                  /* 陀螺仪数据 */
  unsigned short PressureData[PRESSURE_SENSOR_COUNT]; /* 压力传感器数据 */
};

#endif /* PY_PACKAGE_H */