using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Product.FindAsync(id);
    return product is not null ? Results.Ok(products) : Results.NotFound();
});

app.MapPost("/product", async (Product product, AppDbContext db) =>
{
    var productDb = await db.Product.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{productDb.Entity.Id}", productDb.Entity);
});

app.MapPut("/product/{id:int}", async (int id, Product product, ApplicationDbContext db) =>
{
    var productDb = await db.Product.FindAsync(id);
    if (productDb is null)
        return Results.NotFound();

    productDb.Name = product.Name;
    productDb.Description = product.Description;
    productDb.Price = product.Price;
    productDb.ImageUrl = product.ImageUrl;
    db.Product.Update(productDb);
    await db.SaveChangesAsync();
    return Results.Ok(productDb);
});

app.MapDelete("/product/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Product.FindAsync(id);
    if (product is null)
        return Results.NotFound();

    db.Product.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/inventories", async (AppDbContext db) =>
{
    var inventories = await db.Inventories.ToListAsync();
    return Results.Ok(inventories);
});

app.MapGet("/product-inventory/{id:int}", async (int id, AppDbContext db) =>
{
    var productInventory = await db.ProductInventory
        .Include(pi => pi.Produto)
        .Include(pi => pi.Inventory)
        .FirstOrDefaultAsync(pi => pi.Id == id);

    return productInventory is not null ? Results.Ok(productInventory) : Results.NotFound();
});

app.MapPost("/product-inventory", async (ProductInventory productInventory, AppDbContext db) =>
{
    var inventoryDb = await db.ProductInventory.AddAsync(productInventory);
    await db.SaveChangesAsync();
    return Results.Created($"/product-inventory/{inventoryDb.Entity.Id}", inventoryDb.Entity);
});

app.MapPut("/product-inventory/{id:int}", async (int id, ProductInventory productInventory, AppDbContext db) =>
{
    var existingInventory = await db.ProductInventory.FindAsync(id);
    if (existingInventory is null)
        return Results.NotFound();

    existingInventory.Quantity = productInventory.Quantity;
    existingInventory.ProductId = productInventory.ProductId;
    existingInventory.InventoryId = productInventory.InventoryId;
    db.ProductInventory.Update(existingInventory);
    await db.SaveChangesAsync();
    return Results.Ok(existingInventory);
});

app.MapDelete("/product-inventory/{id:int}", async (int id, AppDbContext db) =>
{
    var productInventory = await db.ProductInventory.FindAsync(id);
    if (productInventory is null)
        return Results.NotFound();

    db.ProductInventory.Remove(productInventory);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/inventories/{id:int}", async (int id, AppDbContext db) =>
{
    var inventory = await db.Inventories.FindAsync(id);
    return inventory is not null ? Results.Ok(inventory) : Results.NotFound();
});

app.MapPost("/inventories", async (Inventories inventory, AppDbContext db) =>
{
    var inventoryDb = await db.Inventories.AddAsync(inventory);
    await db.SaveChangesAsync();
    return Results.Created($"/inventories/{inventoryDb.Entity.Id}", inventoryDb.Entity);
});

app.MapPut("/inventories/{id:int}", async (int id, Inventories inventory, AppDbContext db) =>
{
    var existingInventory = await db.Inventories.FindAsync(id);
    if (existingInventory is null)
        return Results.NotFound();

    existingInventory.Name = inventory.Name;
    existingInventory.Location = inventory.Location;
    db.Inventories.Update(existingInventory);
    await db.SaveChangesAsync();
    return Results.Ok(existingInventory);
});

app.MapDelete("/inventories/{id:int}", async (int id, AppDbContext db) =>
{
    var inventory = await db.Inventories.FindAsync(id);
    if (inventory is null)
        return Results.NotFound();

    db.Inventories.Remove(inventory);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}

public class ProductInventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int InventoryId { get; set; }
    public int Quantity { get; set; }

    public Produto Produto { get; set; }
    public Inventories Inventory { get; set; }
}

public class Inventories
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<Product> Product => Set<Product>();
    public DbSet<ProductInventory> ProductInventory => Set<ProductInventory>();
    public DbSet<Inventories> Inventories => Set<Inventories>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}