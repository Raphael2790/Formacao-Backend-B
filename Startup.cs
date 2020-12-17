using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SHOP.Data;

namespace SHOP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            //adiciona a possibilidade chamadas cross-domain
            services.AddCors();
            //adicionando ao services compressão para gerar arquivos zip do tipo JSON
            services.AddResponseCompression(options =>
            {   
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/json"});
            });

            //adiciona cache a toda a aplicação
            //services.AddResponseCaching();

            //adicionando os controller ao services
            services.AddControllers();

            //transformando a chave dentro de settings em bytes
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            //adicionando autenticação ao services
            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            } 
            );
            //adicionando o contexto de banco de dados ao services e aplicando o contexto global
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            //services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));

            //adiocionado documentação para a APi via Swagger
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Shop Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("v1/swagger.json", "Shop Api V1");
            });

            app.UseRouting();

            //tirando politica de restrições para cabeçalhos, metodos http e origens
            app.UseCors( x => x 
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );

            //Ordem muito importante autenticação e autorização
            //Ambos são middlewares dentro da aplicação
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
