﻿global using BitMono.API.Configuration;
global using BitMono.API.Protecting;
global using BitMono.API.Protecting.Analyzing;
global using BitMono.API.Protecting.Contexts;
global using BitMono.API.Protecting.Injection;
global using BitMono.API.Protecting.Renaming;
global using BitMono.API.Protecting.Resolvers;
global using BitMono.Core.Extensions.Configuration;
global using BitMono.Core.Extensions.Protections;
global using BitMono.Core.Protecting.Analyzing.DnlibDefs;
global using BitMono.Core.Protecting.Analyzing.Naming;
global using BitMono.Core.Protecting.Analyzing.TypeDefs;
global using BitMono.Core.Protecting.Attributes;
global using BitMono.Shared.Models;
global using BitMono.Utilities.Extensions.dnlib;
global using dnlib.DotNet;
global using dnlib.DotNet.Emit;
global using Microsoft.Extensions.Configuration;
global using Newtonsoft.Json;
global using NullGuard;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Threading;
global using System.Xml.Serialization;
global using FieldAttributes = dnlib.DotNet.FieldAttributes;
global using TypeAttributes = dnlib.DotNet.TypeAttributes;
global using ILogger = Serilog.ILogger;