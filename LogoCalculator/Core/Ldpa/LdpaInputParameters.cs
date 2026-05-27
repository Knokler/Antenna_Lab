using System;
using System.Collections.Generic;
using System.Text;

namespace LogoCalculator.Core.Ldpa
{
    /// <summary>
    /// Входные параметры для расчет плоской LDPA антенны.
    /// </summary>
    public  class LdpaInputParameters
    {
        /// <summary>
        /// Минимальная частота расчета антенны в МГц.
        /// </summary>
        public double MinFrequencyMHz { get; set; } = 1080;

        /// <summary>
        /// Макстимальная частота расчета антенны в МГц.
        /// </summary>
        public double MaxFrequencyMHz { get; set; } = 6000;

        /// <summary>
        /// Коэфицент масштабирования LDPA.
        /// Каждый следущий элемент короче в ТАУ раз
        /// Обычно 0.86-0.9
        /// </summary>
        public double Tau { get; set; } = 0.9;

        /// <summary>
        /// Относительный интервал между элементами
        /// Обычно 0.04-0.12
        /// </summary>
        public double Sigma { get; set; } = 0.7;

        /// <summary>
        /// Коэфицент укорочения для плоского вибратора.
        /// Для листового элемента можно начать с 0.9-0.94
        /// </summary>
        public double VelocityFactor { get; set; } = 0.92;

        /// <summary>
        /// Режим расчета ширины плоских вибраторов .
        /// </summary>
        public ElementWidthMode WidthMode { get; set; } = ElementWidthMode.ProportionalToLength;

        /// <summary>
        /// Ширина первого элементав мм.
        /// </summary>
        public double FirstElementWidthMm { get; set; } = 8;

        /// <summary>
        /// Cоотношение ширины элемента к его длине, если выбран режим ProportionalToLength.
        /// 0.04 означает 4% от длинны элемента.
        /// </summary>
        public double ElementWidthRatio { get; set; } = 0.04;

        /// <summary>
        /// Минимальная ширина элемента в мм
        /// ограничиваеться технологическим процессом лазерной резки.
        /// </summary>
        public double MinElementWidthMm { get; set; } = 2;

        /// <summary>
        /// Максимальная ширина элемента в мм
        /// </summary>
        public double MaxElementWidthMm { get; set; } = 20;

        /// <summary>
        /// ширина бума в мм.
        /// пока используеться как технологический параметр
        /// </summary>
        public double BoomLengthMm { get; set; } = 6.0;

        /// <summary>
        /// Зазор между бумами в мм.
        /// </summary>
        public double BoomGapMm { get; set; } = 4.0;

        /// <summary>
        /// Количество элементов LDPA минимум 3
        /// </summary>
        public double ElementCount { get; set; } = 2;


    }
}
