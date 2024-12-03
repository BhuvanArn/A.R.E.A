target "client_web" {
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=client_web"
  ]
  cache-from = [
    "type=gha,scope=client_web"
  ]
}
target "server" {
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=server"
  ]
  cache-from = [
    "type=gha,scope=server"
  ]
}
target "db" {
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=db"
  ]
  cache-from = [
    "type=gha,scope=db"
  ]
}
target "client_mobile" {
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=client_mobile"
  ]
  cache-from = [
    "type=gha,scope=client_mobile"
  ]
}
target "service-about" {
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope=service-about"
  ]
  cache-from = [
    "type=gha,scope=service-about"
  ]
}
group "default" {
  targets = ["client_web", "server", "db", "client_mobile", "service-about"]
}