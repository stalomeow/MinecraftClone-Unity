/*
 * @author: Jin Yuhan
 * @date: 2020-12-16 17:06:53
 * @lastTime: 2020-12-26 16:29:22
 */

#include "gyro.h"

Gyro::Gyro(void)
{
  this->dataHandler = NULL;
  this->ucRxCount = 0;

  // memset(&this->dataCache, 0, sizeof(GyroData));
  // memset(this->ucRxBuffer, 0, sizeof(unsigned char) * RX_BUFFER_LEN);
}

void Gyro::Update(unsigned char ucData)
{
  this->ucRxBuffer[this->ucRxCount++] = ucData;

  if (ucRxBuffer[0] != 0x55)
  {
    this->ucRxCount = 0;
    return;
  }

  if (this->ucRxCount == RX_BUFFER_LEN)
  {
    this->ucRxCount = 0;

    switch (this->ucRxBuffer[1])
    {
    case 0x51:
      memcpy(&this->dataCache.Acceleration, &ucRxBuffer[2], sizeof(Vector3));
      break;

    case 0x52:
      memcpy(&this->dataCache.AngularVelocity, &ucRxBuffer[2], sizeof(Vector3));
      break;

    case 0x53:
      memcpy(&this->dataCache.Rotation, &ucRxBuffer[2], sizeof(Vector3));

      if (this->dataHandler)
      {
        // 为 dataCache 创建一个防御性副本
        this->dataHandler(this->dataCache);
      }
      break;
    }
  }
}

void Gyro::OnFinishReceivingData(GyroDataHandler handler)
{
  this->dataHandler = handler;
}