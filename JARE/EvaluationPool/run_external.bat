echo off

echo Starting worker with parameter...
java -cp "lib/log4j-1.2.15.jar;binder.jar;" visnja.ClientExternal %1 %2
echo worker done.
