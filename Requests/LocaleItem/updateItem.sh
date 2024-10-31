#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo ultimo inserimento in update",
    "context": "app",
    "userId": "0",
    "aggregateId": "f8435090-b708-4e21-b22a-f39b2b04f1a2"
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
    "content": "questo veramente ultimo inserimento in update",
    "context": "appero",
    "userId": "0",
    "aggregateId": "f8435090-b708-4e21-b22a-f39b2b04f1a2"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

