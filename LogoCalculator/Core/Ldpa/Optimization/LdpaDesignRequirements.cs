using System;
using System.Collections.Generic;
using System.Text;

namespace LogoCalculator.Core.Ldpa.Optimization
{
    /// <summary>
    /// Требование к проектируемой LDPA антенне.
    /// </summary>
    public class LdpaDesignRequirements
    {
        /// <summary>
        /// Нижняя частота диапозона.
        /// </summary>
        public double MinFrequencyMHz { get; set; } = 1080;
        /// <summary>
        /// Верхняя частота диапозона.
        /// </summary>
        public double MaxFrequencyMHz { get; set; } = 6000;
        
        /// <summary>
        /// Целевой угол раскрытия.
        /// </summary>
        public double TargetApexAngleDeg { get; set; } = 60;

        /// <summary>
        /// Желательный коэффициент усиления в дБи.
        /// </summary>
        public double TargetGainDbi { get; set; } = 7;
        /// <summary>
        /// Максимально допустимая длинна антенны в мм. Если 0, то длинна не ограничена.
        /// </summary>
        public double MaxLenghtMm { get; set; } = 0;
        /// <summary>
        /// Максимальная ширина антенны в мм. Если 0, то ширина не ограничена.
        /// </summary>
        public double MaxWidthMm { get; set; } = 0;

        public double VelocityFactor { get; set; } = 0.95; // Коэффициент укорочения для расчета длинны элементов антенны. Обычно для печатных антенн 0.95, для проволочных 0.98-0.99.
        public ElementWidthMode WidthMode { get; set; } = ElementWidthMode.ProportionalToLength;
        public double ElementWidthRatio { get; set; } = 0.04; // Отношение ширины элемента к его длине, если WidthMode == ProportionalToLength.
        public double MinElementWidthMm { get; set; } = 2; // Минимальная ширина элемента в мм, если WidthMode == Fixed.
        public double MaxElementWidthMm { get; set; } = 12; // Максимальная ширина элемента в мм, если WidthMode == Fixed.

    }
}
