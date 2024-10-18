#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore",
    "context": "app",
    "userId": "0",
    "aggregateId": "bbe58eaf-5e11-428a-8a22-ae9a596fea47"
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
    "content": "ciao amore 88",
    "context": "appero",
    "userId": "0",
    "aggregateId": "bbe58eaf-5e11-428a-8a22-ae9a596fea47"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

