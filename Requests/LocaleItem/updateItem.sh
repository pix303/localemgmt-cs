#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo altro inserimento in update di prova",
    "context": "app",
    "userId": "123",
    "aggregateId": "11781d9c-b1e6-4d61-b0a4-8f8412f7b6da"
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
    "aggregateId": "11781d9c-b1e6-4d61-b0a4-8f8412f7b6da"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

