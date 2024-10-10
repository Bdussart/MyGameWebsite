const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      '/api/Auth/Login',
      '/api/Users'
    ],
    target: "https://localhost:7184",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
