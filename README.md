# rshadercompiler

XNA shader compiler for Xbox 360, originally made by Tser in 2007.

It has been cleaned up somewhat and converted to Visual Studio 2017 and .NET3.0.

This repo also includes the required XNA 1.0 libraries in `libs/`.

## Usage

```
rshadercompiler <input_file> <output_file> <type> [<main>]
where <type> is ps     for Xbox 360 pixel shader
                vs     for Xbox 360 vertex shader
                xvs    for Xbox 360 vertex shader assembly
                xps    for Xbox 360 pixel shader assembly
                effect for effects
      <main> is the name of the entry point; defaults to main
```
