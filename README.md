# Poc Cartões Petz
PoC de emissão de cartões personalizados utilizando análise de imagem (Google Cloud Vision). Desenvolvido para utilização na rede de petshops Petz.

[1 - Página de Upload](https://github.com/devlabdpi/Poc_Cartoes_Petz/blob/master/README.md#1---p%C3%A1gina-de-upload-1)

[2 - Analisar Imagens](https://github.com/devlabdpi/Poc_Cartoes_Petz/blob/master/README.md#2---analisar-imagens-1)

[3 - Buscar Imagens no Repositório](https://github.com/devlabdpi/Poc_Cartoes_Petz/blob/master/README.md#3---buscar-imagens-no-reposit%C3%B3rio-1)

[4 - Apagar Imagens no Repositório](https://github.com/devlabdpi/Poc_Cartoes_Petz/blob/master/README.md#4---apagar-imagens-do-reposit%C3%B3rio)

## 1 - Página de Upload
```
GET https://r37fjpy8x0.execute-api.sa-east-1.amazonaws.com/pagina_upload?TotemID=[ID]
```

A página de upload é visitada pelo usuário ao ler o QR code presente no totem. Nesta página, é feito o upload de até 4 imagens para um repositório temporário hospedado no Google Cloud Storage. 

Cada totem terá seu identificador único, que será especificado através de uma querystring na URL.

#### Exemplo - ID do Totem = 1
https://storage.googleapis.com/poc-cartoes-petz-id-visual/index.html?TotemID=1

## 2 - Analisar Imagens
```
POST https://mtaqbp28md.execute-api.sa-east-1.amazonaws.com/default/poc_petz_analyze
```

Recebe-se uma imagem e realiza a análise, buscando por pessoas, animais, logomarcas e conteúdo inapropriado. Retorna se a imagem é válida ou não. Para a imagem ser válida, é necessário que:
- Contenha algum pet
- Não contenha pessoas ou rostos
- Não contenha logomarcas
- Não contenha conteúdo inapropriado (violência, nudez)

#### Corpo da requisição
- ***TotemID (long)***: Identificador único do Totem
- ***NumeroImagem (short)***: Número de imagem, de 1 a 4
- ***ImagemBase64 (string)***: Imagem codificada em string base64, ***sem*** URL Encoding

Exemplo:
```
{
  "TotemID": 42,
  "NumeroImagem":1,
  "ImagemBase64":"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"
}
```

#### Corpo da resposta
- ***TotemID (long)***: Identificador único do Totem
- ***NumeroImagem (short)***: Número de imagem, de 1 a 4
- ***Sucesso (boolean)***: Condição que indica se o processo ocorreu com sucesso (imagem adequada e salva no repositório)
- ***Descricao (string)***: Informação acerca do resultado da requisição, mais utilizado para erros
- ***ConteudoSeguro (boolean)***: Condição que indica se a imagem não apresenta conteúdo inapropriado (Safe Search do Google)
- ***ContemAnimal (boolean)***: Condição que indica se a imagem apresenta pets
- ***ContemLogomarca (boolean)**: Condição que indica se a imagem apresenta logomarcas
- ***ContemPessoa (boolean)***: Condição que indica se a imagem apresenta pessoas ou rostos
- ***MediaLink (string)***: URL para acesso à imagem no repositório

Exemplo:
```
{
  "TotemID":42,
  "NumeroImagem":1,
  "Sucesso": true,
  "Descricao": "Imagem OK - salva no repositório",
  "ConteudoSeguro":true,
  "ContemAnimal":true,
  "ContemLogomarca":false,
  "ContemPessoa":false,
  "MediaLink": "[...]"
}
```

## 3 - Buscar Imagens no Repositório
```
POST https://mtaqbp28md.execute-api.sa-east-1.amazonaws.com/default/poc_petz_get_images
```

Acessa o repositório de um totem especificado no corpo de uma requisição e retorna uma lista com os ***media links*** das imagens salvas (até 4).

#### Corpo da requisição
- ***TotemID (long)***: Identificador único do Totem

Exemplo:
```
{
  "TotemID":42
}
```

#### Corpo da resposta
Lista de itens com os seguintes atributos:
- ***Numero (short)***: Número da imagem, de 1 a 4
- ***MediaLink (string)***: URL de acesso à imagem para download
- ***DataUpload (string)***: Data e horário do upload da imagem no repositório

Exemplo:
```
[
  {
    "Numero": 1,
    "MediaLink": "https://storage.googleapis.com/download/storage/v1/b/analiseimagempetz-000001/o/1.jpeg?generation=1590773765355857&alt=media",
    "DataUpload": "2020-05-29T17:36:05.355+00:00"
  },
  {
    "Numero": 2,
    "MediaLink": "https://storage.googleapis.com/download/storage/v1/b/analiseimagempetz-000001/o/2.jpeg?generation=1590773768177898&alt=media",
    "DataUpload": "2020-05-29T17:36:08.177+00:00"
  },
  {
    "Numero": 3,
    "MediaLink": "https://storage.googleapis.com/download/storage/v1/b/analiseimagempetz-000001/o/3.jpeg?generation=1590773770766016&alt=media",
    "DataUpload": "2020-05-29T17:36:10.765+00:00"
  },
  {
    "Numero": 4,
    "MediaLink": "https://storage.googleapis.com/download/storage/v1/b/analiseimagempetz-000001/o/4.jpeg?generation=1590773774298153&alt=media",
    "DataUpload": "2020-05-29T17:36:14.297+00:00"
  }
]
```

## 4 - Apagar Imagens do Repositório
```
POST https://mtaqbp28md.execute-api.sa-east-1.amazonaws.com/default/poc_petz_clear_bucket
```

Apaga as imagens salvas no repositório do totem especificado.

#### Corpo da requisição
- ***TotemID (long)***: Identificador único do Totem

Exemplo:
```
{
  "TotemID":42
}
```

#### Corpo da resposta
Lista de itens com os seguintes atributos:
- ***TotemID (long)***: Identificador único do totem
- ***Sucesso (boolean)***: Condição que define se a operação foi bem-sucedida
- ***Descricao (string)***: texto informativo acerca da resposta

Exemplo:
```
{
  "TotemID":42
  "Sucesso": true,
  "Descricao": "Repositório esvaziado com sucesso."
}
```
