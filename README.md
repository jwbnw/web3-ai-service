## WIP Solana Hyperdrive Hackathon Project

Service Layer For Web3 AI

Readme is WIP and may be incomplete at times. Reach out to repo owner with questions.


## Local Dev Getting Started

1. Clone this repository 

2. [Trust the HTTPS development certificate](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-7.0&tabs=visual-studio%2Clinux-ubuntu#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos) and place it in ./../Resources/Certs/localhost.pfx or modify appsetting.json locally, removing or replacing the entry depending on your preferred local set-up and do not commit

3. Run the application locally under the https profile
```bash
dotnet run --launch-profile https
```

Open [https://localhost:7247/swagger/index.html](https://localhost:7247/swagger/index.html) to load swagger

## Dev Resources 

- [.NET7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

- I highly recommend either using VS Code or Visual Studio

- If having trouble creating a local cert see [This Stackoverflow page](https://stackoverflow.com/questions/55485511/how-to-run-dotnet-dev-certs-https-trust)
