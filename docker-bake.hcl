target "client_web" {
  context = "./client"
  dockerfile = "Dockerfile"
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=client_web"
  ]
  cache-from = [
    "type=gha,scope=client_web"
  ]
}
target "server" {
  context = "./services/router"
  dockerfile = "Dockerfile"
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=server"
  ]
  cache-from = [
    "type=gha,scope=server"
  ]
}
target "client_mobile" {
  context = "./mobile"
  dockerfile = "Dockerfile"
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=client_mobile"
  ]
  cache-from = [
    "type=gha,scope=client_mobile"
  ]
}
target "service-about" {
  context = "./services/about"
  dockerfile = "Dockerfile"
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=service-about"
  ]
  cache-from = [
    "type=gha,scope=service-about"
  ]
}
group "default" {
  targets = ["client_web", "server", "client_mobile", "service-about"]
}