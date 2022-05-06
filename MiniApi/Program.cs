using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MiniApi.data;
using MiniApi.models;

var builder = WebApplication.CreateBuilder(args);

string urlCliente = "/clientes";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>
    (options => options.UseMySql("server=localhost;initial catalog=zeus;uid=projetoNovo;pwd=projetoNovo",
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();


app.MapGet(urlCliente, async (AppDbContext dbContext) =>
        dbContext.Clientes.Where(a => a.Id != 0));

//OBTER//
app.MapGet(urlCliente + "/{id}", async (int id, AppDbContext dbContext) =>
      await dbContext.Clientes.FirstOrDefaultAsync(a => a.Id == id));


//INSERIR//
app.MapPost("/Clientes", async (Cliente cliente, AppDbContext dbContext) =>
{

    dbContext.Clientes.Add(cliente);
    await dbContext.SaveChangesAsync();
    return cliente;
});


//EDITAR//
app.MapPut("/Clientes/{id}", async (int id, Cliente cliente, AppDbContext dbContext) =>
{
    dbContext.Entry(cliente).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return cliente;
});

//DELETAR//
app.MapDelete("/Clientes/{id}", async (int id, AppDbContext dbContext) =>
{
    var cliente = await dbContext.Clientes.FirstOrDefaultAsync(a => a.Id == id);
    if (cliente != null)
    {
        dbContext.Clientes.Remove(cliente);
        await dbContext.SaveChangesAsync();
    }
    return;
});

app.UseSwaggerUI();

await app.RunAsync();