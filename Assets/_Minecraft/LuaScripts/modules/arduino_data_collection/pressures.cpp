/*
 * @author: Jin Yuhan
 * @date: 2020-12-09 17:10:10
 * @lastTime: 2020-12-26 14:48:09
 */

#include "pressures.h"

Pressures::Pressures(uint8_t *sensorPins)
{
  memcpy(this->sensorPins, sensorPins, PRESSURE_SENSOR_COUNT * sizeof(uint8_t));
}

void Pressures::ReadData(unsigned short *buffer) const
{
  for (int i = 0; i < PRESSURE_SENSOR_COUNT; i++)
  {
    *buffer++ = (unsigned short)analogRead(this->sensorPins[i]);
  }
}