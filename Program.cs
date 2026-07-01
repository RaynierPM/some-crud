using someCrud.bootstrap.rpc;

AppWrapper.init(args);

var app = AppWrapper.instance.app;

app.UseHttpsRedirection();
app.Run();