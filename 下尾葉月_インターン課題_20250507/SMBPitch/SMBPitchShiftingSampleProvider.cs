using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NAudio.Wave;

namespace 下尾葉月_インターン課題_20250507
{
    public class SMBPitchShiftingSampleProvider : ISampleProvider
    {
        //Shifter objects
        private ISampleProvider SourceStream = null;
        private WaveFormat WFormat = null;
        private float Pitch = 1f;
        private int _FFTSize;
        private long _osamp;
        private SMBPitchShifter ShifterLeft = new SMBPitchShifter();
        private SMBPitchShifter ShifterRight = new SMBPitchShifter();

        //Limiter constants
        const float LIM_THRESH = 0.95f;
        const float LIM_RANGE = (1f - LIM_THRESH);
        const float M_PI_2 = (float)(Math.PI / 2);

        public SMBPitchShiftingSampleProvider(ISampleProvider SourceProvider)
            : this(SourceProvider, 4096, 4L, 1f) { }

        public SMBPitchShiftingSampleProvider(ISampleProvider SourceProvider, int FFTSize, long osamp, float InitialPitch)
        {
            SourceStream = SourceProvider;
            WFormat = SourceProvider.WaveFormat;
            _FFTSize = FFTSize;
            _osamp = osamp;
            PitchFactor = InitialPitch;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int SampRead = SourceStream.Read(buffer, offset, count);
            if (Pitch == 1f)
                //Nothing to do.
                return SampRead;
            if (WFormat.Channels == 1)
            {
                float[] Mono = new float[SampRead];
                int index = 0;
                for (int sample = offset; sample <= SampRead + offset - 1; sample++)
                {
                    Mono[index] = buffer[sample];
                    index += 1;
                }
                ShifterLeft.PitchShift(Pitch, SampRead, _FFTSize, _osamp, WFormat.SampleRate, Mono);
                index = 0;
                for (int sample = offset; sample <= SampRead + offset - 1; sample++)
                {
                    buffer[sample] = Limiter(Mono[index]);
                    index += 1;
                }
                return SampRead;
            }
            else if (WFormat.Channels == 2)
            {
                float[] Left = new float[(SampRead >> 1)];
                float[] Right = new float[(SampRead >> 1)];
                int index = 0;
                for (int sample = offset; sample <= SampRead + offset - 1; sample += 2)
                {
                    Left[index] = buffer[sample];
                    Right[index] = buffer[sample + 1];
                    index += 1;
                }
                ShifterLeft.PitchShift(Pitch, SampRead >> 1, _FFTSize, _osamp, WFormat.SampleRate, Left);
                ShifterRight.PitchShift(Pitch, SampRead >> 1, _FFTSize, _osamp, WFormat.SampleRate, Right);
                index = 0;
                for (int sample = offset; sample <= SampRead + offset - 1; sample += 2)
                {
                    buffer[sample] = Limiter(Left[index]);
                    buffer[sample + 1] = Limiter(Right[index]);
                    index += 1;
                }
                return SampRead;
            }
            else
            {
                throw new Exception("Shifting of more than 2 channels is currently not supported.");
            }
        }

        public NAudio.Wave.WaveFormat WaveFormat
        {
            get { return WFormat; }
        }

        public float PitchFactor
        {
            get { return Pitch; }
            set
            {
                Pitch = value;
            }
        }

        private float Limiter(float Sample)
        {
            float res = 0f;
            if ((LIM_THRESH < Sample))
            {
                res = (Sample - LIM_THRESH) / LIM_RANGE;
                res = (float)((Math.Atan(res) / M_PI_2) * LIM_RANGE + LIM_THRESH);
            }
            else if ((Sample < -LIM_THRESH))
            {
                res = -(Sample + LIM_THRESH) / LIM_RANGE;
                res = -(float)((Math.Atan(res) / M_PI_2) * LIM_RANGE + LIM_THRESH);
            }
            else
            {
                res = Sample;
            }
            return res;
        }
    }
}
