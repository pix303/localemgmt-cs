#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo altro inserimento in update di prova",
    "context": "app",
    "userId": "123",
    "aggregateId": "9d4a4d94-5608-42cd-a7df-3491f05a6eb"
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
    "aggregateId": "9d4a4d94-5608-42cd-a7df-3491f05a6eb"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

