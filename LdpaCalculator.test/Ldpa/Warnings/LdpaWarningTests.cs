using LogoCalculator.Core.Ldpa;
using LdpaCalc = LogoCalculator.Core.Ldpa.LdpaCalculator;

namespace LdpaCalculator.test.Ldpa.Warnings
{
    public class LdpaWarningTests
    {
        [Fact]
        public void Calculate_WhenApexAngleIsAcceptable_ShouldNotAddLargeApexAngleWarning()
        {
            // Тест если угол нормальный то варнинг не нужен
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,

                // Более спокойная геометрия
                Tau = 0.90,
                Sigma = 0.07,

                VelocityFactor = 0.95,
                MinElementWidthMm = 1.0,
                MaxElementWidthMm = 30.0
            };

            // Act
            LdpaResult result = calculator.Calculate(input);

            // Assert
            Assert.True(result.ApexAngleDeg <= 60.0);

            Assert.DoesNotContain(
                result.Warnings,
                warning => warning.Contains("угол раскрыва"));
        }
        [Fact]
        public void Calculate_WhenApexAngelsIsTooLarge_ShouldAddWarning()
        {
            //Тест 28: если угол раскрыва слишком большой — должен быть warning
            //Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                //маленький тау и маленький угол раскрыва
                Tau = 0.6,
                Sigma = 0.04,

                VelocityFactor = 0.95,
                MinElementWidthMm = 1.0,
                MaxElementWidthMm = 30.0
            };
            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            Assert.Contains(
                result.Warnings,
                warning => warning.Contains("угол раскрыва"));
        }

        [Fact]
        public void Calculate_WhenShortestElementIsLargeEnoughComparedToMinWidth_ShouldNotAddShortElementWarning()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 8,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.95,

                MinElementWidthMm = 1.0,
                MaxElementWidthMm = 30.0
            };

            // Act
            LdpaResult result = calculator.Calculate(input);

            // Assert
            Assert.True(
                result.ShortestElementLengthMm >= input.MinElementWidthMm * 5.0);

            Assert.DoesNotContain(
                result.Warnings,
                warning => warning.Contains("короткий элемент"));
        }
        [Fact]
        public void Calculate_WhenShortestElementIsTooSmallComparedToMinWidth_ShouldAddWarning()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.6,
                Sigma = 0.07,
                VelocityFactor = 0.95,

                MinElementWidthMm = 20.0,
                MaxElementWidthMm = 30.0
            };

            // Act
            LdpaResult result = calculator.Calculate(input);

            // Assert
            Assert.Contains(
                result.Warnings,
                warning => warning.Contains("короткий элемент"));
        }
        [Fact]
        public void Calculate_WhenHighestFrequencyIsLessThanMaxFrequency_ShouldAddWarnings()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 4,
                Tau = 0.6,
                Sigma = 0.07,
                VelocityFactor = 0.95
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            Assert.Contains(
                result.Warnings,
                warning => warning.Contains("верхней частоты")
                );
        }

        [Fact]
        public void Calculate_WhenHighestFrequencyCoversMaxFrequency_ShouldNotAddUpperFrequencyWarning()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.6,
                Sigma = 0.07,
                VelocityFactor = 0.95
            };

            // Act
            LdpaResult result = calculator.Calculate(input);

            // Assert
            double highestFrequency = result.Elements.Max(element => element.FrequencyMHz);

            Assert.True(highestFrequency >= input.MaxFrequencyMHz);

            Assert.DoesNotContain(
                result.Warnings,
                warning => warning.Contains("верхней частоты"));
        }
    }
}
