import yaml
from sys import exit

docker_compose_file = 'docker-compose.yml'
docker_bake_file = 'docker-bake.hcl'

def generate_docker_bake():
    with open(docker_compose_file, 'r') as file:
        compose_data = yaml.safe_load(file)

    targets = []
    bake_content = ""

    for service_name, service_data in compose_data['services'].items():
        if service_name == 'db':
            continue

        context = service_data['build']['context']
        dockerfile = service_data['build']['dockerfile']

        target = f"""
target "{service_name}" {{
  context = "{context}"
  dockerfile = "{dockerfile}"
  cache-to = [
    "type=gha,ignore-error=true,mode=max,scope={service_name}"
  ]
  cache-from = [
    "type=gha,scope={service_name}"
  ]
}}
"""
        targets.append(f'"{service_name}"')
        bake_content += target

    group = f"""
group "default" {{
  targets = [{', '.join(targets)}]
}}
"""
    bake_content += group

    with open(docker_bake_file, 'w') as file:
        file.write(bake_content)

    print(f"Generated {docker_bake_file} successfully.")

if __name__ == "__main__":
    generate_docker_bake()
    exit(0)
