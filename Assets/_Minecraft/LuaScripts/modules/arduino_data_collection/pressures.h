/*
 * @author: Jin Yuhan
 * @date: 2020-11-25 14:07:19
 * @lastTime: 2020-12-26 14:17:02
 */

#ifndef PRESSURES_H
#define PRESSURES_H

#include <Arduino.h>

/**
 * 压力传感器的数量
 */
#define PRESSURE_SENSOR_COUNT 6

/**
 * 表示一组压力传感器
 */
class Pressures
{
public:
  /**
   * 创建一组压力传感器的实例
   * @param[in] sensorPins 一个数组，保存所有连接了压力传感器的引脚的编号
   */
  Pressures(uint8_t *sensorPins);
  /**
   * 从压力传感器中读取数据
   * @param[in] buffer 保存数据的缓冲区，其长度应该与压力传感器的数量一致
   */
  void ReadData(unsigned short *buffer) const;

private:
  uint8_t sensorPins[PRESSURE_SENSOR_COUNT]; /* 所有连接了压力传感器的引脚的编号 */
};

#endif /* PRESSURES_H */
