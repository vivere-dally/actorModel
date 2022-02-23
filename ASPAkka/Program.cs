using Akka.Actor;
using ASPAkka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(_ => ActorSystem.Create("ASKAkka"));
builder.Services.AddSingleton<UserActorProvider>((provider) =>
{
    var actorSystem = provider.GetService<ActorSystem>();
    var userActor = actorSystem!.ActorOf(UserActor.Props());
    return () => userActor;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetService<ActorSystem>();
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Services.GetService<ActorSystem>()!.Terminate().Wait();
});

app.Run();
