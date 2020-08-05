# Manufacturer shortcuts for CryoFall (Working Hotkeys for all manufacturer structures)

Mod adds ability to use Container menu hotkeys for all manufacturer structures (stoves, furnaces, oil refinery, oil cracking plant and so on, even for mulchbox and wells!).

For **Oil refinery** and **Oil cracking plant** it works a little bit differ: it adds empty canisters and canisters of needed type into input slots even if input slots are empty. 

For all other manufacturers mod adds items into input slot `only if any of input slots already has item of this type inside`.

![image](https://i.imgur.com/06K9ezS.png)

There is no hint for this keys inside manufacturer's window like we have for crates, but just press it and it will work.

**Mod type**: Client-side mod.

**How to install mod**:

Please read instructions in [Automaton's topic](https://forums.atomictorch.com/index.php?topic=1097.msg6646#msg6646)

If you don't have any other mods result file should look like this:
Code: 
```
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<mods>
  <mod>core_1.0.0</mod>
  <mod>ManufacturerShortcuts</mod>
</mods>
```

or if you have another mods installed:

```
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<mods>
  <mod>core_1.0.0</mod>
   ... other mods lines
  <mod>ManufacturerShortcuts</mod>
</mods>```
