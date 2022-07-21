using System.Threading.Tasks;
using CrudSample.Core.Command;
using CrudSample.Core.Models;
using CrudSample.Core.Queries;
using FluentAssertions;
using Moq;
using Xunit;

namespace ProductApi.Test;

public class CommandTests
{
    [Fact]
    public async Task CreateProduct_WithValidCommand_ShouldBeSuccessful()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(p => p.Create(It.IsAny<Product>())).Verifiable();

        var handler = new CreateProductCommandHandler(mockRepo.Object);

        var createdProduct = await handler.Handle(new CreateProductCommand()
        {
            Name = "Test product",
            Price = 10,
            Description = "Look, the test product description"
        });

        createdProduct.Name.Should().Be("Test product");
        createdProduct.PricingHistory.Count.Should().Be(1);
        createdProduct.Description.Should().Be("Look, the test product description");
        mockRepo.Verify(p => p.Create(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateProduct_WithInvalidName_ShouldNotCreate()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(p => p.Create(It.IsAny<Product>())).Verifiable();

        var handler = new CreateProductCommandHandler(mockRepo.Object);

        await handler.Handle(new CreateProductCommand()
        {
            Name = null,
            Price = 10
        });
        
        mockRepo.Verify(p => p.Create(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateProduct_WithInvalidPrice_ShouldNotCreate()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(p => p.Create(It.IsAny<Product>())).Verifiable();

        var handler = new CreateProductCommandHandler(mockRepo.Object);

        await handler.Handle(new CreateProductCommand()
        {
            Name = "Test product",
            Price = 0
        });
        
        mockRepo.Verify(p => p.Create(It.IsAny<Product>()), Times.Never);
    }
}