window.sqlIntellisense = {
    suggestions: [],

    registerProvider: function () {
        if (monaco) {
            monaco.languages.registerCompletionItemProvider('sql', {
                provideCompletionItems: function (model, position) {
                    var word = model.getWordUntilPosition(position);
                    var range = {
                        startLineNumber: position.lineNumber,
                        endLineNumber: position.lineNumber,
                        startColumn: word.startColumn,
                        endColumn: word.endColumn
                    };

                    return {
                        suggestions: window.sqlIntellisense.suggestions.map(s => ({
                            label: s,
                            kind: monaco.languages.CompletionItemKind.Field,
                            insertText: s,
                            range: range
                        }))
                    };
                }
            });
        }
    },

    updateSuggestions: function (newSuggestions) {
        this.suggestions = newSuggestions;
    }
};
