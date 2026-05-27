using LdpaCalc = LogoCalculator.Core.Ldpa.LdpaCalculator;
using LogoCalculator.Core.Ldpa;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace LdpaCalculator.test.Ldpa.Calculation
{
    public class LdpaCalculatorTests
    {
        [Fact]
        public void Calculate_ApexAngleShouldBeDoubleAlphaDeg()
        {
            //Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };
            //Act
            LdpaResult result = calculator.Calculate(input);
            //Assert
            
            Assert.Equal(
                result.AlphaDeg * 2.0,
                result.ApexAngleDeg,
                precision: 6);
        }

        [Fact]
        public void Calculate_ShouldCalcalateAlphaDeg()
        {
            //Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act

            LdpaResult result = calculator.Calculate(input);

            //Assert

            double expectedAlphaRad = Math.Atan((1.0 - input.Tau)/(4.0 * input.Sigma));
            double expectedAlphaDeg = expectedAlphaRad * (180.0 / Math.PI);

            Assert.Equal(
                expectedAlphaDeg, 
                result.AlphaDeg, 
                precision: 6);
        }
        [Fact]
        public void Calculate_TotalLengthShouldBeLastElementXCenter()
        { 
            //Arrange
            var calculator = new LdpaCalc();
            
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert

            Assert.Equal(
                result.Elements[^1].XCenterMm,
                result.TotalLengthMm,
                precision: 6);

        }

        [Fact]

        public void Calculate_TotalWidthShouldBeLongestElementLength()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            Assert.Equal(
                result.LongestElementLengthMm,
                result.TotalWidthMm,
                precision: 6);
        }

        [Fact]
        public void Calculate_ShortestElementShouldBeLastElementLength()
        {
            //Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };
            //Act
            LdpaResult result = calculator.Calculate(input);
            //Assert
            Assert.Equal(
                result.Elements[^1].FullLengthMm,
                result.ShortestElementLengthMm,
                precision: 6);
        }
        [Fact]
        public void Calculate_LongestElementShouldBeFirstElementLength()
        { 
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert

            Assert.Equal(
                result.Elements[0].FullLengthMm,
                result.LongestElementLengthMm,
                precision: 6);
        }
        [Fact]
        public void Calculate_WidthShouldNotBeGreaterThanMaximumWidth()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92,

                WidthMode = ElementWidthMode.ProportionalToLength,
                ElementWidthRatio = 0.5, // This would give very wide widths

                MinElementWidthMm = 1.0,
                MaxElementWidthMm = 10.0 // Set maximum width to 20mm
            };

            //Act

            LdpaResult result = calculator.Calculate(input);

            //Assert

            foreach(var element in result.Elements)
            {
                Assert.True(
                    element.WidthMm <= input.MaxElementWidthMm,
                    $"Ширина элемента {element.Index}превышает максимальную.");
            }
        }
        [Fact]
        public void Calculate_WidthShouldNotBeLessThanMinimumWidth()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92,

                WidthMode = ElementWidthMode.ProportionalToLength,
                ElementWidthRatio = 0.01, // This would give very narrow widths
                
                MinElementWidthMm = 2.0, // Set minimum width to 2mm
                MaxElementWidthMm = 20.0
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            foreach(var element in result.Elements)
            {
                Assert.True(
                    element.WidthMm >= input.MinElementWidthMm,
                    $"Ширина элемента {element.Index}меньше минимальной.");
             
            }
        }
        [Fact]
        public void Calculate_WithProportionalWidth_ShouldCalculateWidthFromLe()
        {
            //Arrange
            var calculatyor = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92,
                WidthMode = ElementWidthMode.ProportionalToLength,
                ElementWidthRatio = 0.04,
                MinElementWidthMm = 1.0,
                MaxElementWidthMm = 20.0
            };

            //Act
            LdpaResult result = calculatyor.Calculate(input);

            //Assert
            foreach (var element in result.Elements)
            {
                // Assert logic here
                double expectedWidth = element.FullLengthMm * input.ElementWidthRatio;

                Assert.Equal(
                    expectedWidth,
                    element.WidthMm,
                    precision: 6

                    );
            }
        }
        [Fact]
        public void Calculate_ShouldAlternatePhaseReservedFlag() 
        { 
            //Arrange 
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert

            for(int i=1; i < result.Elements.Count; i++)
            {
                // Assert logic here
                bool expected = i % 2 == 1;

                Assert.Equal(expected, result.Elements[i].IsPhaseReversed);
            }
        }

        [Fact]

        public void Calculate_XCenterShouldAccumulateSpacing()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };
            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            Assert.Equal(0.0, result.Elements[0].XCenterMm, precision: 6);

            double expectedX = 0.0;

            for (int i = 1; i < result.Elements.Count; i++)
            {
                expectedX += result.Elements[i - 1].SpacingToNextMm;

                Assert.Equal(
                    expectedX,
                    result.Elements[i].XCenterMm,
                    precision: 6
                    );
            }
        }
        [Fact]
        public void Calculate_SpacingToNextShouldUseSigma()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            foreach (var element in result.Elements)
            {
                double expectedSpacing = 2.0 * input.Sigma * element.FullLengthMm;

                Assert.Equal(
                    expectedSpacing,
                    element.SpacingToNextMm,
                    precision: 6);
            }
        }
        [Fact]
        public void Calculate_ArmLengthShouldBeHalfOfFullLength()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };
            //Act
            LdpaResult result = calculator.Calculate(input);
            //Assert
            foreach (var element in result.Elements)
            {
                Assert.Equal(
                    element.FullLengthMm / 2.0, 
                    element.ArmLengthMm,
                    precision: 6);
            }
        }
        [Fact]
        public void Calculate_ShouldCreateExpectedNumberOfElements()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            /*var input = new LdpaInputParameters
            {
                ElementCount = 16
            };
            */
            //Act

            LdpaResult result = calculator.Calculate(input);

            //Assert

            Assert.Equal(16, result.Elements.Count);
        }
        [Fact]

        public void Calculate_ShouldNumberElementsFromOne()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act
            LdpaResult result = calculator.Calculate(input);

            //Assert
            Assert.Equal(1, result.Elements[0].Index);
            Assert.Equal(16, result.Elements[^1].Index);
        }

        [Fact]

        public void Calculate_EachNextElementShouldBeShourter()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };

            //Act

            LdpaResult result = calculator.Calculate(input);

            //Assert

            for (int i = 1; i < result.Elements.Count; i++)
            {
                Assert.True
                    (
                        result.Elements[i].FullLengthMm < result.Elements[i - 1].FullLengthMm,
                           $"Элемент {i + 1} должен быть короче элемента {i}"
                        );
            }
        }

        [Fact]

        /// <summary>
        /// Тест на частоту первого элемента. Она должна быть близка к минимальной частоте, заданной в параметрах.
        /// </summary>
        public void Calculate_FirstElementFrequencyShouldBeCloseToMinFrequency()
        {
            //Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0.92
            };
            //Act
            LdpaResult result = calculator.Calculate(input);
            //Assert


            Assert.InRange(result.Elements[0].FrequencyMHz, 1070, 1090);
        }
    }
}
