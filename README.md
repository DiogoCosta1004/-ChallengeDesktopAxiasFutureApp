#  Axia Desktop Client (.NET 8.0)

Este é um aplicativo desktop construído em **.NET 8.0**, que oferece autenticação com API externa, recepção de mensagens em tempo real via **WebSocket**, leitura automática das mensagens com **TTS (Text-to-Speech)**, e controle de volume integrado — tudo por meio de uma interface gráfica intuitiva.

---

## ✅ Funcionalidades

### 🔐 Autenticação via Login
- Interface gráfica para entrada de **usuário** e **senha**
- Integração com o endpoint:
POST https://beta.axiafutures.com/api/mock-login

- Fluxo de autenticação:
- Envia as credenciais em JSON para o backend
- Se `200 OK`: acesso autorizado
- Se `404 Not Found`: exibe erro ao usuário

### 📡 Feed de Mensagens com WebSocket
- Conexão ativa com:
wss://edge-api.axiafutures.com/ws/?token=U2FsdGVkX1+YcfF5A506hKmuKwlK2a4WErOATfH/Ek9GtuMmtY0FbGqnH892r4B8

- Tratamento de mensagens:
- `ping`: responde com `pong` automaticamente
- `message`: exibe o conteúdo na interface e envia para leitura via TTS

### 🗣️ Leitor de Texto com TTS e Controle de Volume
- As mensagens recebidas são lidas automaticamente com voz sintetizada
- Controle de volume ajustável via interface
- Ideal para acessibilidade e leitura em segundo plano

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia         | Descrição                              |
|--------------------|------------------------------------------|
| .NET 8.0           | Plataforma base                         |
|  WPF     | Interface gráfica  |
| System.Net.WebSockets | Conexão WebSocket nativa            |
| System.Speech.Synthesis | TTS (Text-to-Speech) da Microsoft |
| HttpClient         | Autenticação via API REST               |

---

## 🧪 Credenciais para Teste

Use as seguintes credenciais no formulário de login:

| Campo     | Valor     |
|-----------|-----------|
| Username  | `user`    |
| Password  | `pass123` |

---

## 🚀 Como Executar

### 1. Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado

### 2. Clonar o projeto
```bash
git clone https://github.com/seu-usuario/axia-desktop-client.git
cd axia-desktop-client



