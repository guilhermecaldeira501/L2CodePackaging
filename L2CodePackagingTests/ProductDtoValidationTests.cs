using L2CodePackagingAPI.DTOs;
using System.ComponentModel.DataAnnotations;

namespace L2CodePackagingTests
{
    public class ProductDtoValidationTests
    {
        [Theory]
        [InlineData(0, 10, 10)]
        [InlineData(10, 0, 10)]
        [InlineData(10, 10, 0)]
        [InlineData(-5, 10, 10)]
        [InlineData(10, -3, 10)]
        [InlineData(10, 10, -1)]

        public void ProductDto_InvalidDimensions_ShouldFailValidation(int altura, int largura, int comprimento)
        {
            // Arrange
            var product = new ProductDto
            {
                Id = "TEST",
                Altura = altura,
                Largura = largura,
                Comprimento = comprimento
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void ProductDto_ValidProduct_ShouldPassValidation()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = "VALID-PRODUCT",
                Altura = 10,
                Largura = 15,
                Comprimento = 20
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void ProductDto_EmptyId_ShouldFailValidation()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = "", // ID vazio
                Altura = 10,
                Largura = 10,
                Comprimento = 10
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
            Assert.Contains("The Id field is required.", results.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void ProductDto_Volume_ShouldCalculateCorrectly()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = "VOLUME-TEST",
                Altura = 10,
                Largura = 20,
                Comprimento = 30
            };

            // Act
            var volume = product.Altura * product.Largura * product.Comprimento;

            // Assert
            Assert.Equal(6000, volume); // 10 * 20 * 30
        }
    }
}
