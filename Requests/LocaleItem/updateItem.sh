#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo altro inserimento in update di prova",
    "context": "app",
    "userId": "123",
    "aggregateId": "8e1d967d-3350-4b3d-8982-5168d4097d5a"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .



json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo veramente ultimo inserimento in update di prova!!!!!!",
    "context": "appero",
    "userId": "1230",
    "aggregateId": "8e1d967d-3350-4b3d-8982-5168d4097d5a"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .


json_data=$(
  cat <<EOF
{
    "lang": "en",
    "content": "this is a test really!",
    "context": "appero",
    "userId": "1230",
    "aggregateId": "8e1d967d-3350-4b3d-8982-5168d4097d5a"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .


json_data=$(
  cat <<EOF
{
    "lang": "en",
    "content": "this is a test really! moddeddddd",
    "context": "appero",
    "userId": "1230xxx",
    "aggregateId": "8e1d967d-3350-4b3d-8982-5168d4097d5a"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

