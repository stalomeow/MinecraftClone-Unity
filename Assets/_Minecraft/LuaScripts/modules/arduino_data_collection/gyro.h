/*
 * @author: Jin Yuhan
 * @date: 2020-11-25 17:05:23
 * @lastTime: 2020-12-26 16:28:59
 */

#ifndef GYRO_H
#define GYRO_H

#include <Arduino.h>

/**
 * RX数据缓冲区的长度
 */
#define RX_BUFFER_LEN 11

/**
 * 表示一个三维的向量
 */
struct Vector3
{
  short X; /* 向量的X分量 */
  short Y; /* 向量的Y分量 */
  short Z; /* 向量的Z分量 */
};

/**
 * 表示从陀螺仪传感器接收的一组数据
 */
struct GyroData
{
  Vector3 Acceleration;    /* 加速度 */
  Vector3 AngularVelocity; /* 角速度 */
  Vector3 Rotation;        /* 旋转 */
};

/**
 * 表示用来处理陀螺仪数据的函数
 * @param[in] data 陀螺仪数据
 */
typedef void (*GyroDataHandler)(GyroData data);

/**
 * 表示一个陀螺仪传感器
 */
class Gyro
{
public:
  /**
   * 创建一个陀螺仪传感器的实例
   */
  Gyro(void);

  /**
   * 更新陀螺仪数据
   * @param[in] ucData 从串口中读取的一个字节
   * @note 这个方法需要在loop函数中不断调用
   */
  void Update(unsigned char ucData);

  /**
   * 设置用来处理接收完成的陀螺仪数据的函数
   * @param[in] handler 陀螺仪数据的处理函数
   */
  void OnFinishReceivingData(GyroDataHandler handler);

private:
  GyroData dataCache;                      /* 陀螺仪数据缓存 */
  GyroDataHandler dataHandler;             /* 陀螺仪数据处理函数 */
  unsigned char ucRxCount;                 /* 从RX读取的字节数量 */
  unsigned char ucRxBuffer[RX_BUFFER_LEN]; /* RX数据缓冲区 */
};

#endif /* GYRO_H */
