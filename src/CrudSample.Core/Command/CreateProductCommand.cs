using CrudSample.Core.DataTransfer;
using CrudSample.Core.Models;

namespace CrudSample.Core.Command;

public record CreateProductCommand
{
    public string Name { get; set; }
    
    public decimal Price { get; set; }
    
    public string Description { get; set; }

    internal bool ValidateProperties()
    {
        return !string.IsNullOrEmpty(Name) && Price > 0;
    }
}

public class CreateProductCommandHandler
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDTO> Handle(CreateProductCommand command)
    {
        if (!isCommandValid(command))
        {
            return null;
        }

        var product = Product.Create(command.Name, command.Price);
        
        product.UpdateDescription(command.Description);

        await this._productRepository.Create(product);

        return new ProductDTO(product);
    }

    private bool isCommandValid(CreateProductCommand command)
    {
        if (command == null)
        {
            return false;
        }

        return command.ValidateProperties();
    }
}