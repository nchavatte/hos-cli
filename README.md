This command line tool is based upon [hos-lib](https://github.com/nchavatte/hos-lib) to provide file serialization/deserialization.

# Usage

## Serialization

```shell
> hos-cli serialize source.bin
```
returns the [serial form](https://github.com/nchavatte/hos-lib/wiki/Serial-form) of `source.bin` to the standard output.

## Deserialization

```shell
> hos-cli deserialize serial-form.txt output.bin
```
converts the [serial form](https://github.com/nchavatte/hos-lib/wiki/Serial-form) that is in `serial-form.txt` to bytes and write them into `output.bin`.
