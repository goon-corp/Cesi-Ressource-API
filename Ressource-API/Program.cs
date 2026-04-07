using DotNetEnv;
using Ressource_API.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

Env.Load();


// Building WebApp with all the dependencies and middlewares needed
builder.BuildSolution();