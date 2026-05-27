using System;
using System.Collections.Generic;
using System.Text;

namespace LogoCalculator.Core.Ldpa
{
    /// <summary>
    /// Сервіс расчета классической Ldpa антенны.
    /// </summary>
    public class LdpaCalculator
    {


        /// <summary>
        /// Расчет угла раскрыва антенны в градусах на основе параметров tau и sigma.
        /// </summary>
        /// <param name="tau"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        private static double CalculateAlphaDeg(double tau, double sigma)
        {
            double alphaRad = Math.Atan((1.0 - tau) / (4.0 * sigma));
            return alphaRad*180.0 / Math.PI; // конвертация в градусы
        }
       /// <summary>
       /// Расчет длинны элементов
       /// </summary>
       /// <param name="input">Объект с параметрами антенны</param>
       /// <param name="fullLengthMm">Полная длина элемента в мм</param>
       /// <param name="elementIndex">Номер элемента</param>
       /// <returns></returns>
        private static double CalculateElementWidthMm(
            LdpaInputParameters input,
            double fullLengthMm,
            int elementIndex
            )
        { 
            double widthMm = input.WidthMode switch
            {
                ElementWidthMode.Constant => input.FirstElementWidthMm, 

                ElementWidthMode.ScaledByTau => input.FirstElementWidthMm * Math.Pow(input.Tau, elementIndex),

                ElementWidthMode.ProportionalToLength => fullLengthMm * input.ElementWidthRatio,

                ElementWidthMode.Custom => input.FirstElementWidthMm,

                _ => input.FirstElementWidthMm
            };

            return Math.Clamp(
                widthMm,
                input.MinElementWidthMm,
                input.MaxElementWidthMm
                );
        }
        /// <summary>
        /// Скорость света в мм/с
        /// </summary>
        private const double SpeedOfLightMmPerSec = 299_792_458_000.0; // скорость света в мм/с

        /*
        public LdpaResult Calculate(LdpaInputParameters input)
        {
            ValidateInput(input);

            var result = new LdpaResult();

            for (int i = 0; i < input.ElementCount; i++)
            {
                result.Elements.Add(new LdpaElement
                {
                    Index = i + 1
                });
            }

            return result;
        }
        */

        /// <summary>
        /// Блок расчета классической Ldpa антенны на основе входных параметров.
        /// </summary>
        /// <param name="input">Входные параметры для расчета Ldpa антенны.</param>
        /// <returns></returns>
        public LdpaResult Calculate(LdpaInputParameters input)
        { 
            ValidateInput(input);

            var result = new LdpaResult();
            
            double fMinHz = input.MinFrequencyMHz * 1_000_000.0; // конвертация МГц в Гц

            double firstElementLenghtMm = SpeedOfLightMmPerSec * input.VelocityFactor / (2.0 * fMinHz) ;

            double xCenterMm = 0.0; // TODO: расчет координаты центра элемента по оси X

            for (int i=0; i<input.ElementCount; i++) 
            { 
                double fullLengthMm = firstElementLenghtMm * Math.Pow(input.Tau, i);

                double armLengthMm = fullLengthMm / 2.0; // длина одного плеча вибратора в мм

                double frequencyMHz = SpeedOfLightMmPerSec * input.VelocityFactor / (2.0 * fullLengthMm) / 1_000_000.0;

                double spacingToNextMm = 2.0 * input.Sigma * fullLengthMm; // TODO: расчет расстояния до следующего элемента

               double widthMm = CalculateElementWidthMm(input, fullLengthMm, i);


                result.Elements.Add(new LdpaElement
                {
                    Index = i + 1,
                    FullLengthMm = fullLengthMm,
                    ArmLengthMm = armLengthMm,
                    FrequencyMHz = frequencyMHz,
                    SpacingToNextMm = spacingToNextMm,
                    XCenterMm = xCenterMm,
                    IsPhaseReversed = (i % 2 == 1), // чередование фазы для каждого элемента
                    WidthMm = widthMm
                });

                xCenterMm += spacingToNextMm; // обновляем координату центра для следующего элемента
            }

            result.LongestElementLengthMm = result.Elements.Max(e => e.FullLengthMm);
            result.ShortestElementLengthMm = result.Elements.Min(e => e.FullLengthMm);
            result.TotalWidthMm = result.LongestElementLengthMm;
            result.TotalLengthMm = result.Elements[^1].XCenterMm;
            result.AlphaDeg = CalculateAlphaDeg(input.Tau, input.Sigma);
            result.ApexAngleDeg = 2.0 * result.AlphaDeg;

            AddWarnings(input, result);

            return result;
        }
        /// <summary>
        /// Метод проверки коректностии ввода параметров
        /// Вызывает исключение при не правильном вводе параметров
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidateInput(LdpaInputParameters input)
        { 
            if (input.MinFrequencyMHz <=0)
                throw new ArgumentException("Минимальная частота должна быть больше нуля.");
            
            if (input.MaxFrequencyMHz < input.MinFrequencyMHz)
                throw new ArgumentException("Максимальная частота не должна быть меньще митнимальной");

            if (input.ElementCount < 2)
                throw new ArgumentException("Количество элементов должнобыть не меньше 2");

            if (input.Tau <= 0)
                throw new ArgumentException("Tau должен быть в диапозоне от 0 < Tau < 1 ");

            if (input.Sigma <= 0)
                throw new ArgumentException("Sigma должен быть больше 0");

            if (input.MinElementWidthMm <= 0)
                throw new ArgumentException("Минимальный размер элемента должен быть больше 0");
            
            if(input.Tau <=0 || input.Tau >= 1)
                throw new ArgumentException("Tau должен быть в диапозоне от 0 < Tau < 1 "); // проверка диапозона тау
            
            if (input.VelocityFactor <= 0 || input.VelocityFactor > 1)
                throw new ArgumentException("Коэффициент укорочения должен быть в диапазоне 0 < K <= 1.");
        }

        private static void AddWarnings(LdpaInputParameters input, LdpaResult result) 
        {
            double highestCalculatedFrequency = result.Elements.Max(e => e.FrequencyMHz);

            if (highestCalculatedFrequency < input.MaxFrequencyMHz)
            {
                result.Warnings.Add("Количество элементов не достаточно для перекрытия верхней частоты диапозона.");
            }

            if (result.ShortestElementLengthMm < input.MinElementWidthMm * 5)
            {
                result.Warnings.Add("Самый короткий элемент слишком мал относительно минимальной технологической длинны.");
            }

            if (result.ApexAngleDeg > 60.0)
            {
                result.Warnings.Add("Геометричный угол раскрыва слишком большой. Возможны ухудшения направлености и согласования.");
            }
        }
        /*
        public LdpaResult Calculate(LdpaInputParameters input)
        {
            ValidateInput(input);
            var result = new LdpaResult();
            
            
            
            
            
            return  result;
        }
        */

    }
}
