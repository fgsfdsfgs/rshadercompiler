//          The R 360 XNA Shader Compiler (by Tser)
//
//          This Compiler generates binary code out of text shader code, using the effect of the XNA Frameworks
//          Note, the compiler is inside a dll mapped to extern located at
//          "X":\Program Files\Common Files\microsoft shared\XNA\Framework\v1.0X\XNANative1.dll
//          This dll hosts functions like CompileEffectForXbox
//          This allows you to easely precompile binary shader code and
//          use it with other xbox360 projects (like Linux , XBMC)
//
//          History 25-08-22??? Creation, Tser                                     
//                  12 Oct 2020 Modified by fgsfds          

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rshadercompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("R 360 XNA Shader / Effect Compiler\n");
                    Console.WriteLine("usage: rshadercompiler <input_file> <output_file> <type> [<main>]");
                    Console.WriteLine("       where <type> is ps     for Xbox 360 pixel shader ");
                    Console.WriteLine("                       vs     for Xbox 360 vertex shader");
                    Console.WriteLine("                       xvs    for Xbox 360 vertex shader assembly");
                    Console.WriteLine("                       xps    for Xbox 360 pixel shader assembly");
                    Console.WriteLine("                       effect for effects");
                    Console.WriteLine("             <main> is the name of the entry point; defaults to main");

                }
                else
                {
                    string nameofMainFunction = "main";
                    if (args.Length > 3)
                    {
                        nameofMainFunction = args[3];
                    }
                    switch (args[2])
                    {
                        case "ps":
                            ShaderCompiler(args[0], args[1], ShaderProfile.PS_3_0, false, nameofMainFunction);
                            break;
                        case "vs":
                            ShaderCompiler(args[0], args[1], ShaderProfile.VS_3_0, false, nameofMainFunction);
                            break;
                        case "xps":
                            ShaderCompiler(args[0], args[1], ShaderProfile.XPS_3_0, true, string.Empty);
                            break;
                        case "xvs":
                            ShaderCompiler(args[0], args[1], ShaderProfile.XVS_3_0, true, string.Empty);
                            break;
                        case "effect":
                            EffectParser(args[0], args[1]);
                            break;
                        default:
                            Console.WriteLine("Unknown shader type: " + args[2]);
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message.ToString());
                Console.WriteLine(ex.StackTrace.ToString()); // main stack
            }
        }

        static void ShaderCompiler(string inputfile, string outputfile, ShaderProfile shaderProfile, bool isAsm, string entryname)
        {
            CompilerMacro[] macroArray = null;
            macroArray = new CompilerMacro[2];
            macroArray[0].Name = "XBOX";
            macroArray[1].Name = "XBOX360";

            CompiledShader compiledShader;

            if (isAsm)
            {
                compiledShader = Microsoft.Xna.Framework.Graphics.ShaderCompiler.AssembleFromFile(inputfile, macroArray, null, CompilerOptions.None, TargetPlatform.Xbox360);
            }
            else
            {
                compiledShader = Microsoft.Xna.Framework.Graphics.ShaderCompiler.CompileFromFile(inputfile, macroArray, null, CompilerOptions.None, entryname, shaderProfile, TargetPlatform.Xbox360);

            }

            if (compiledShader.Success)
            {
                System.IO.File.WriteAllBytes(outputfile, compiledShader.GetShaderCode());
            }
            else
            {
                Console.WriteLine("Error compiling shader:");
            }

            if (compiledShader.ErrorsAndWarnings != string.Empty)
            {
                Console.WriteLine(compiledShader.ErrorsAndWarnings);
            }
        }

        static void EffectParser(string inputfile, string outputfile)
        {
            CompilerMacro[] macroArray = null;
            macroArray = new CompilerMacro[2];
            macroArray[0].Name = "XBOX";
            macroArray[1].Name = "XBOX360";
            CompiledEffect compiledEffect = Microsoft.Xna.Framework.Graphics.Effect.CompileEffectFromFile(inputfile, macroArray, null, CompilerOptions.None, TargetPlatform.Xbox360);

            if (compiledEffect.Success)
            {
                System.IO.File.WriteAllBytes(outputfile, compiledEffect.GetEffectCode());
            }
            else
            {
                Console.WriteLine("Error compiling effect:");
            }

            if (compiledEffect.ErrorsAndWarnings != string.Empty)
            {
                Console.WriteLine(compiledEffect.ErrorsAndWarnings);
            }
        }
    }
}
