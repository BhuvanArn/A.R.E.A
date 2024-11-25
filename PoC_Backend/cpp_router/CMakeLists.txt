cmake_minimum_required(VERSION 3.10)
project(CppServer)

set(CMAKE_CXX_STANDARD 17)

find_package(Boost 1.70 REQUIRED COMPONENTS system)

add_executable(CppServer main.cpp)
target_include_directories(CppServer PRIVATE ${Boost_INCLUDE_DIRS})
target_link_libraries(CppServer PRIVATE ${Boost_LIBRARIES})
