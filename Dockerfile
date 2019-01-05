FROM microsoft/dotnet:sdk

WORKDIR /app
COPY . ./

RUN cd DeployApi.Tests && dotnet test && cd ..
RUN cd DeployApi && dotnet restore && dotnet publish -c Release -o out && cd ..

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=0 /app/DeployApi/out .
ENTRYPOINT [ "dotnet", "DeployApi.dll" ]
